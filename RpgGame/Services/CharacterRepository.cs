using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RpgGame.DbContexts;
using RpgGame.DTOs.Character;
using RpgGame.Models;

namespace RpgGame.Services
{
    public class CharacterRepository : ICharacterRepository, IDisposable
    {
        private readonly GameContext _context;

        public CharacterRepository(GameContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Character> GetCharacters()
        {
            return _context.Characters.ToList<Character>();
        }

        public Character GetCharacter(Guid characterId)
        {
            if (characterId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(characterId));
            }
            return _context.Characters.FirstOrDefault(c => c.Id == characterId);
        }

        public void AddCharacter(Character character)
        {
            if (character == null)
            {
                throw new ArgumentNullException(nameof(character));
            }

            character.Id = Guid.NewGuid();
            _context.Characters.Add(character);
        }

        public void UpdateCharacter(Character character)
        {
            throw new NotImplementedException();
        }

        public void DeleteCharacter(Character character)
        {
            if (character == null)
            {
                throw new ArgumentNullException(nameof(character));
            }

            _context.Characters.Remove(character);
        }

        public bool TaskExists(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return _context.Characters.Any(c => c.Id == id);
        }
        
        

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}