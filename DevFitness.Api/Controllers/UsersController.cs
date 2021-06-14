using AutoMapper;
using DevFitness.Api.Core.Entities;
using DevFitness.Api.Models.InputModels;
using DevFitness.Api.Models.ViewModels;
using DevFitness.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevFitness.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DevFitnessDbContext _dbContext;
        private readonly IMapper _mapper;
        public UsersController(DevFitnessDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        /// <summary>
        /// Retornar detalhes de usuario
        /// </summary>
        /// <param name="id">Identificador de usuario</param>
        /// <returns>Objeto de detalhes do usuario</returns>
        /// <response code="404">Usuario não encontrado</response>
        /// <response code="200">Usuario encontrado</response>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Id == id);

            if(user == null)
            {
                return NotFound();
            }

            // var userViewModel = new UserViewModel(user.Id, user.FullName, user.Height, user.Weight, user.BirthDate);

            var userViewModel = _mapper.Map<UserViewModel>(user);

            return Ok(userViewModel);
        }

        // api/users  método HTTP POST
        /// <summary>
        /// Cadastrar um usuário
        /// </summary>
        /// <remarks>
        /// Requisição de exemplo: 
        ///{
        /// "fullName":"Teste",
        /// "height":1.75,
        /// "weight":70,
        /// "birthDate":"1995-02-12 00:00:00"
        /// }
        /// </remarks>
        /// <param name="inputModel">Objeto com dados de cadastro de Usuário</param>
        /// <returns>Objeto recém-criado.</returns>
        /// <response code="201">Objeto criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        public IActionResult Post([FromBody] CreateUserInputModel inputModel)
        {
            // var user = new User(inputModel.FullName, inputModel.Height, inputModel.Weight, inputModel.BirthDate);

            var user = _mapper.Map<User>(inputModel);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(Get),new { id = user.Id}, inputModel);
        }

        /// <summary>
        /// Atualizar dados do usuario
        /// </summary>
        /// <remarks>
        /// Requisição de exemplo: 
        ///{
        /// "height":1.75,
        /// "weight":70,
        /// }
        /// </remarks>
        /// <param name="id">Identificador de usuario</param>
        /// <param name="inputModel">Objeto com dados de atualização do Usuário</param>
        /// <returns>Objeto Atualizado</returns>
        /// <response code="201">Objeto atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateUserInputModel inputModel)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Update(inputModel.Height, inputModel.Weight);

            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
