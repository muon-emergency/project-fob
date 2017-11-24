using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using project_fob.Models;

namespace project_fob.DAL {
    public class FobInitializer : System.Data.Entity.CreateDatabaseIfNotExists<FobContext>{
        
        protected override void Seed(FobContext context) {
            //call testdata here to populate
        }
        
    }
}