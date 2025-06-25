﻿using Identity.Application.Data.Repositories;
using Identity.Domain.Exceptions;

namespace Identity.Application.Identity.Queries.GetProfile
{
    public sealed class GetProfileHandler : IQueryHandler<GetProfileQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetProfileHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetProfileQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(query.UserId);
            if (user == null)
            {
                throw new DomainException("User not found", 404, "The requested user could not be located in the system.");
            }

            var roles = await _userRepository.GetRolesAsync(user);

            return new UserDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber,
                user.BirthDate,
                user.Gender.ToString(),
                user.SelfIntroduction,
                user.CreatedAt,
                roles.ToList(),
                user.GetImageUrlsList()
            );
        }
    }
}