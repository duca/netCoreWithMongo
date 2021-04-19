using System;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Api.Data.Collections
{
    public class Infectado
    {
        public Infectado(string ref_, DateTime dataNascimento_, string sexo_, double latitude_, double longitude_)
        {
            this.Ref = ref_;
            this.DataNascimento = dataNascimento_;
            this.Sexo = sexo_;
            this.Localizacao = new GeoJson2DGeographicCoordinates(longitude_, latitude_);
        }

        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public GeoJson2DGeographicCoordinates Localizacao { get; set; }
        public string Ref { get; set; }
    }
}