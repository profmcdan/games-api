using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgGame.DTOs.Character;
using RpgGame.Helpers;
using RpgGame.Models;
using RpgGame.Services;

namespace RpgGame.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/characters")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;
        
        public CharacterController(ICharacterRepository characterRepository, IMapper mapper)
        {
            _characterRepository = characterRepository ?? throw new ArgumentNullException(nameof(characterRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<ServiceResponse<IEnumerable<GetCharacterDto>>> GetCharacters()
        {
            ServiceResponse<IEnumerable<GetCharacterDto>> serviceResponse =
                new ServiceResponse<IEnumerable<GetCharacterDto>>();
            var characters = _characterRepository.GetCharacters();
            serviceResponse.Data = _mapper.Map<IEnumerable<GetCharacterDto>>(characters);
            return Ok(serviceResponse);
        }

        [HttpGet("{characterId}", Name = "GetCharacter")]
        public ActionResult<ServiceResponse<GetCharacterDto>> GetSingleCharacter(Guid characterId)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            var character = _characterRepository.GetCharacter(characterId);
            if (character == null)
            {
                return NotFound();
            }
            response.Data = _mapper.Map<GetCharacterDto>(character);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<ServiceResponse<GetCharacterDto>> AddCharacter(AddCharacterDto newCharacter)
        {
            // ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            var characterEntity = _mapper.Map<Character>(newCharacter);
            _characterRepository.AddCharacter(characterEntity);
            _characterRepository.Save();
            var characterToReturn = _mapper.Map<GetCharacterDto>(characterEntity);
            return CreatedAtRoute("GetCharacter", new {characterId = characterToReturn.Id}, characterToReturn);
        }

        [HttpPut("{characterId}")]
        public  ActionResult<GetCharacterDto> UpdateCharacter(Guid characterId, UpdateCharacterDto payload)
        {
            var character = _characterRepository.GetCharacter(characterId);
            if (character == null)
            {
                return NotFound();
            }
            _characterRepository.UpdateCharacter(character);
            // if (response.Data == null)
            // {
            //     return StatusCode(500, response);
            // }
            
            return Ok();
        }
        
        [HttpDelete]
        public  ActionResult<GetCharacterDto> Delete(Guid characterId)
        {
            var character = _characterRepository.GetCharacter(characterId);
            if (character == null)
            {
                return NotFound();
            }
            _characterRepository.DeleteCharacter(character);
            return Ok();
        }
    }
}