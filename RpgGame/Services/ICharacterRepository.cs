using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RpgGame.DTOs.Character;
using RpgGame.Models;

namespace RpgGame.Services
{
    public interface ICharacterRepository
    {
        IEnumerable<Character> GetCharacters();
        Character GetCharacter(Guid characterId);
        void AddCharacter(Character character);
        void UpdateCharacter(Character character);
        void DeleteCharacter(Character character);
        bool TaskExists(Guid id);
        bool Save();
    }
}