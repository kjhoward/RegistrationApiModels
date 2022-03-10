using System.Collections.Generic;

namespace JustHelpDesk.Models{
    public class QueryResult<T>{
        public int TotalItems {get;set;}
        public IEnumerable<T> Items {get;set;}
    }
}