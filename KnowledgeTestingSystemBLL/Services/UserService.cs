using AutoMapper;
using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Entities.DTO;
using KnowledgeTestingSystemBLL.Interfaces;
using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>().ReverseMap();
                cfg.CreateMap<User, ApplicationUser>().ReverseMap();
                cfg.CreateMap<UserProfile, UserProfileDTO>().ReverseMap();
                cfg.CreateMap<UserProfileTest, UserProfileTestDTO>().ReverseMap();
            });
            _mapper = new Mapper(config);
        }

        public async Task CreateAsync(UserDTO entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var user = _mapper.Map<UserDTO, User>(entity);

            await _unitOfWork.UserRepository.AddAsync(user);

            var createdUser = await _unitOfWork.UserRepository.GetByEmailAsync(user.Email);

            await _unitOfWork.UserProfileRepository.AddAsync(new UserProfile
            {
                UserId = createdUser.Id
            });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var userToDelete = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (userToDelete == null)
                throw new ArgumentException("There is no such user");

            await _unitOfWork.UserRepository.DeleteByIdAsync(id);
            var profileId = (await _unitOfWork.UserProfileRepository.GetByUserIdAsync(id)).Id;
            await _unitOfWork.UserProfileRepository.DeleteByUserIdAsync(id);
            await _unitOfWork.UserProfileTestRepository.DeleteByUserProfileIdAsync(profileId);
            return true;
        }

        public async Task<UserDTO> EditAsync(UserDTO entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            var mappedUser = _mapper.Map<UserDTO, User>(entity);
            await _unitOfWork.UserRepository.UpdateAsync(mappedUser);

            var updatedUser = await _unitOfWork.UserRepository.GetByIdAsync(entity.Id);
            var resultEntity = _mapper.Map<User, UserDTO>(updatedUser);
            return resultEntity;
        }

        public async Task<UserCompleteInformation> EditCompleteAsync(UserCompleteInformation entity)
        {
            if(entity is null)
                throw new ArgumentNullException(nameof(entity));

            var mappedUser = _mapper.Map<UserDTO, User>(entity.User);
            await _unitOfWork.UserRepository.UpdateAsync(mappedUser);

            var mappedUserProfile = _mapper.Map<UserProfileDTO, UserProfile>(entity.UserProfile);
            await _unitOfWork.UserProfileRepository.UpdateAsync(mappedUserProfile);

            UserCompleteInformation resultEntity = new UserCompleteInformation();
            var updatedUser = await _unitOfWork.UserRepository.GetByIdAsync(entity.User.Id);
            resultEntity.User = _mapper.Map<User, UserDTO>(updatedUser);

            var updatedUserProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(entity.UserProfile.Id);
            resultEntity.UserProfile = _mapper.Map<UserProfile, UserProfileDTO>(updatedUserProfile);

            resultEntity.UserProfileTest = entity.UserProfileTest;
            return resultEntity;
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await _unitOfWork.UserRepository.GetAllAsync());

            return users;
        }

        public async Task<IEnumerable<UserDTO>> GetAsync(Func<UserDTO, bool> filter)
        {
            var users = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await _unitOfWork.UserRepository.GetAllAsync());

            return users.Where(filter).ToList();
        }

        public async Task<UserCompleteInformation> GetWithProfileAsync(int id)
        {
            var user = _mapper.Map<User,UserDTO> (await _unitOfWork.UserRepository.GetByIdAsync(id));
            if(user is null) throw new ArgumentNullException(nameof(user));

            var userProfile = _mapper.Map<UserProfile, UserProfileDTO>(await _unitOfWork.UserProfileRepository.GetByUserIdAsync(id));
            if(userProfile is null) throw new ArgumentNullException(nameof(userProfile));

            var userProfilesTests = (await _unitOfWork.UserProfileTestRepository.GetByUserIdAsync(id)).ToArray();
            UserProfileTestDTO[] userProfilesTestsDTO = new UserProfileTestDTO[userProfilesTests.Length];
            for (int i = 0; i < userProfilesTests.Length; i++)
            {
                userProfilesTestsDTO[i] = _mapper.Map<UserProfileTest,UserProfileTestDTO>(userProfilesTests[i]);
            }
            UserCompleteInformation userInfo = new UserCompleteInformation()
            {
                User = user,
                UserProfile = userProfile,
                UserProfileTest = userProfilesTestsDTO != null ? userProfilesTestsDTO : new UserProfileTestDTO[0]
            };
            return userInfo;
        }

        public async Task AddUserProfileTest(Result result, string userEmail)
        {
            var user = await GetAsync(x => x.Email == userEmail);
            var userProfile = await _unitOfWork.UserProfileRepository.GetByUserIdAsync(user.ElementAt(0).Id);

            var oldUserProfileTest = (await _unitOfWork.UserProfileTestRepository.GetByUserIdAsync(user.ElementAt(0).Id))
                                     .Where(x => x.TestId == result.TestId);

            if(oldUserProfileTest.Count() != 0)
            {
                oldUserProfileTest.ElementAt(0).Grade = result.Grade;
                oldUserProfileTest.ElementAt(0).NumberOfAttempts++;
                await _unitOfWork.UserProfileTestRepository.UpdateAsync(oldUserProfileTest.ElementAt(0));
                return;
            }

            UserProfileTestDTO userProfileTest = new UserProfileTestDTO();
            userProfileTest.UserProfileId = userProfile.Id;
            userProfileTest.Grade = result.Grade;
            userProfileTest.NumberOfAttempts = 1;
            userProfileTest.TestId = result.TestId;
            var userProfileTestToCreate = _mapper.Map<UserProfileTestDTO, UserProfileTest>(userProfileTest);

            await _unitOfWork.UserProfileTestRepository.AddAsync(userProfileTestToCreate);
        }
    }
}
