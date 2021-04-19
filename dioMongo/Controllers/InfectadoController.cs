using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dioMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfectadoController : ControllerBase
    {
        Api.Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Api.Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto_)
        {
            var newRef = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            var infectado = new Infectado(newRef, dto_.DataNascimento, dto_.Sexo, dto_.Latitude, dto_.Longitude);

            _infectadosCollection.InsertOne(infectado);

            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();

            return Ok(infectados);
        }

        [HttpPut]
        public ActionResult AtualizarInfectado ([FromBody] InfectadoDto dto_)
        {
            var filtro = Builders<Infectado>.Filter.Where(alvo => alvo.DataNascimento == dto_.DataNascimento);
            var newOne = Builders<Infectado>.Update.Set(_ => _.DataNascimento, dto_.DataNascimento)
                .Set(_ => _.Sexo, dto_.Sexo)
                .Set(_ => _.Localizacao, new GeoJson2DGeographicCoordinates(dto_.Latitude, dto_.Longitude));

            _infectadosCollection.UpdateOne(filtro, newOne);
            return Ok("Atualizado com sucesso");
        }

        [HttpDelete("{ref_}")]
        public ActionResult RemoverInfectado (string ref_)
        {
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(_ => _.Ref == ref_));

            return Ok("Elemento removido " + ref_);
        }
    }
}
