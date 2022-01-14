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
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<UserProfile, UserProfileDTO>();
                cfg.CreateMap<UserProfileTest, UserProfileTestDTO>();
            });
            _mapper = new Mapper(config);
        }

        public async Task CreateAsync(UserDTO entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var user = new User
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email
            };

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
                return false;

            await _unitOfWork.UserRepository.DeleteByIdAsync(id);

            return true;
        }

        public async Task<UserDTO> EditAsync(UserDTO entity)
        {
            var mappedUser = _mapper.Map<UserDTO, User>(entity);
            await _unitOfWork.UserRepository.UpdateAsync(mappedUser);

            var updatedUser = await _unitOfWork.UserRepository.GetByIdAsync(entity.Id);
            var resultEntity = _mapper.Map<User, UserDTO>(updatedUser);
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
            var userProfile = _mapper.Map<UserProfile, UserProfileDTO>(await _unitOfWork.UserProfileRepository.GetByUserIdAsync(id));
            UserProfileTestDTO userProfileTest = new UserProfileTestDTO();
            try
            {
                userProfileTest = _mapper.Map<UserProfileTest, UserProfileTestDTO>(await _unitOfWork.UserProfileTestRepository.GetByUserIdAsync(id));
            }
            catch (Exception ex)
            {
            }
            UserCompleteInformation userInfo = new UserCompleteInformation()
            {
                User = user,
                UserProfile = userProfile,
                UserProfileTest = userProfileTest,
            };
            return userInfo;
        }
    }
}
