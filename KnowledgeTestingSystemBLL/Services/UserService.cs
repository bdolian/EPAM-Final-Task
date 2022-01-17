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

        /// <summary>
        /// This method creates the user and its profile in application DB
        /// </summary>
        /// <param name="entity">User to create</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">User is null</exception>
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

        /// <summary>
        /// This method deletes all information about the user(UserProfile, UserProfileTests and User itself) 
        /// </summary>
        /// <param name="id">Id of user to delete</param>
        /// <returns>True if everything is fine</returns>
        /// <exception cref="ArgumentException">There is no such user or it's deleted</exception>
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

        /// <summary>
        /// This method edits information about only the User
        /// </summary>
        /// <param name="entity">New User information</param>
        /// <returns>Edited user</returns>
        /// <exception cref="ArgumentNullException">Passed model is null</exception>
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
        /// <summary>
        /// This method edits all user information(User and UserProfile)
        /// </summary>
        /// <param name="entity">Information to edit</param>
        /// <returns>Edited complete user information</returns>
        /// <exception cref="ArgumentNullException">Passed info is null</exception>
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

        /// <summary>
        /// This method gets all users that corresponds the filter(some condition)
        /// </summary>
        /// <param name="filter">Condition for users</param>
        /// <returns>List of users that corresponds the filter</returns>
        public async Task<IEnumerable<UserDTO>> GetAsync(Func<UserDTO, bool> filter)
        {
            var users = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await _unitOfWork.UserRepository.GetAllAsync());

            return users.Where(filter).ToList();
        }

        /// <summary>
        /// This method returns complete user information
        /// </summary>
        /// <param name="id">Id of user to find</param>
        /// <returns>Complete user information</returns>
        /// <exception cref="ArgumentNullException">User not found or is deleted</exception>
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

        /// <summary>
        /// This method adds new entity of UserProfileTest after the user passed the test
        /// </summary>
        /// <param name="result">Result of the test</param>
        /// <param name="userEmail">Email of user that passed it</param>
        /// <returns></returns>
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
