using AutoMapper;
using DevFitness.Api.Core.Entities;
using DevFitness.Api.Models.InputModels;
using DevFitness.Api.Models.ViewModels;
using DevFitness.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DevFitness.Api.Controllers
{
    [Route("api/users/{userId}/meals")]
    public class MealsController : ControllerBase
    {
        private readonly DevFitnessDbContext _dbContext;
        private readonly IMapper _mapper;
        public MealsController(DevFitnessDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Buscar todas refeições do usuario
        /// </summary>
        /// <param name="userId">Identificador do usuario</param>
        /// <returns>Objeto de detalhes da refeição</returns>
        /// <response code="404">Refeições não encontrada</response>
        /// <response code="200">Refeições encontrada</response>

        [HttpGet]
        public IActionResult GetAll(int userId)
        {
            var allMeals = _dbContext.Meals.Where(m => m.UserId == userId && m.Active == true);

            var allMealsViewModel = allMeals
                .Select(m => new MealViewModel(m.Id, m.Description, m.Calories, m.Date));

            return Ok(allMealsViewModel);
        }

        /// <summary>
        /// Buscar uma refeição especifica do usuario
        /// </summary>
        /// <param name="userId">Identificador do usuario</param>
        /// <param name="mealId">Identificador da refeição</param>
        /// <returns>Objeto de detalhes da refeição solicitada</returns>
        /// <response code="404">Refeição não encontrada</response>
        /// <response code="200">Refeição encontrada</response>
        [HttpGet("{mealId}")]
        public IActionResult Get(int userId, int mealId)
        {
            var meal = _dbContext.Meals.SingleOrDefault(m => m.Id == mealId && m.UserId == userId);

            if(meal == null)
            {
                return NotFound();
            }

            // var mealViewModel = new MealViewModel(meal.Id, meal.Description, meal.Calories, meal.Date);

            var mealViewModel = _mapper.Map<MealViewModel>(meal);

            return Ok(mealViewModel);
        }

        /// <summary>
        /// Cadastrar uma nova refeição para o usuario
        /// </summary>
        /// <remarks>
        /// Requisição de exemplo: 
        ///{
        /// "description":"Teste",
        /// "calories":400,
        /// "date":"2021-02-12 00:00:00"
        /// }
        /// </remarks>
        /// <param name="userId">Identificador do usuario</param>
        /// <param name="inputModel">Objeto com dados de cadastro da refeição</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Objeto criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>

        [HttpPost]
        public IActionResult Post(int userId, [FromBody] CreateMealInputModel inputModel)
        {

            //var meal = new Meal(inputModel.Description, inputModel.Calories, inputModel.Date, userId);
            var meal = _mapper.Map<Meal>(inputModel);

            _dbContext.Meals.Add(meal);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { userId= userId, mealId= meal.Id}, inputModel);
        }

        /// <summary>
        /// Atualizar dados da refeição
        /// </summary>
        /// <remarks>
        /// Requisição de exemplo: 
        ///{
        /// "description":"Teste 2",
        /// "calories":700,
        /// "date":"2021-02-12 00:00:00"
        /// }
        /// </remarks>
        /// <param name="userId">Identificador do usuario</param>
        /// <param name="mealId">Identificador da refeição</param>
        /// <param name="inputModel">Objeto com dados de atualização da refeição</param>
        /// <returns>Objeto Atualizado</returns>
        /// <response code="201">Objeto atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPut("{mealId}")]
        public IActionResult Put(int userId,int mealId, [FromBody] UpdateMealInputModel inputModel)
        {
            var meal = _dbContext.Meals.SingleOrDefault(m => m.UserId == userId && m.Id == mealId);

            if (meal == null)
            {
                return NotFound();
            }

            meal.Update(inputModel.Description, inputModel.Calories, inputModel.Date);
            _dbContext.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletar a refeição de um usuario
        /// </summary>
        /// <param name="userId">Identificador do usuario</param>
        /// <param name="mealId">Identificador da refeição</param>
        /// <returns>Refeição deletada</returns>
        /// <response code="204">Objeto deletado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpDelete("{mealId}")]
        public IActionResult Delete(int userId, int mealId)
        {
            var meal = _dbContext.Meals.SingleOrDefault(m => m.UserId == userId && m.Id == mealId);

            if (meal == null)
            {
                return NotFound();
            }

            meal.Deactivate();

            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
