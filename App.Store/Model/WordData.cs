using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Store.Model
{
    public class WordData
    {
        [Key]   
        public string Full { get; set; }
        public string Prefix { get; set; }
        public string Root { get; set; }
        public string Suffix { get; set; }
    }
}
