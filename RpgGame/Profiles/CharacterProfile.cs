using AutoMapper;
using RpgGame.DTOs.Character;
using RpgGame.Models;

namespace RpgGame.Profiles
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
        }
    }
}