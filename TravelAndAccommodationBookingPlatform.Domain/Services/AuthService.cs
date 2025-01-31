using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Services;

public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _argon2PasswordService;
        private readonly ITokenGeneratorService _jwtGeneratorService;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepository,
            IPasswordService argon2PasswordService,
            ITokenGeneratorService jwtGeneratorService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _argon2PasswordService = argon2PasswordService;
            _jwtGeneratorService = jwtGeneratorService;
            _mapper = mapper;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);

            if (user == null)
            {
                throw new AuthenticationFailedException("Invalid credentials");
            }

            var saltBytes = Convert.FromBase64String(user.Salt);
            if (!_argon2PasswordService.VerifyPassword(loginDto.Password, user.PasswordHash, saltBytes))
            {
                throw new AuthenticationFailedException("Invalid credentials");
            }

            var token = await _jwtGeneratorService.GenerateTokenAsync(user.UserId, user.Username, user.Role);

            return token;
        }

        public async Task SignupAsync(SignupDto signupDto)
        {
            if (await _userRepository.GetUserByUsernameAsync(signupDto.Username) != null)
                throw new ConflictException("User with this Username already exists");
            
            if (await _userRepository.GetUserByEmailAsync(signupDto.Email) != null)
                throw new ConflictException("User with this Email already exists");
            
            var user = _mapper.Map<User>(signupDto);

            user.Salt = Convert.ToBase64String(_argon2PasswordService.GenerateSalt());
            var saltBytes = Convert.FromBase64String(user.Salt);
            user.PasswordHash = _argon2PasswordService.GenerateHashedPassword(signupDto.Password, saltBytes);

            await _userRepository.CreateUserAsync(user);
        }
    }