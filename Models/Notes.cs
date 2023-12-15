using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2Examen3P.Models
{
    public class Notes
    {
        public string id_nota
        {
            get; set;
        }

        public string descripcion
        {
            get; set;
        }

        public DateTime fecha
        {
            get; set;
        }

        public byte[] photo_record
        {
            get; set;
        }

        //public string audio_record
        //{
        //    get; set;
        //}
    }
}
