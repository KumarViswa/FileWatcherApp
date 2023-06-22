using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherApp.Model
{

    public class Trade
    {
        [Key]

        public string? TradeID { get; set; }
        public string? ISIN { get; set; }
        public string? Notional { get; set; }


    }
}
