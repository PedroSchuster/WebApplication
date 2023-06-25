using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Persistence.Models
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public PageList()
        {
        }

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;   
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items); // vai adicionar os items da lista que foi passada como parametro pra lista do objeto que está sendo instanciado dessa classe
        }

        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            // esse source vai ser a query de eventos por exemplo, como foi feita la no eventoPersist
            var count = await source.CountAsync(); // conta a quantidade de registros que tem na tabela
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(); 
            // vai skipar o numero de itens de uma pagina * o numero da pagina e vai pegar os itens da pagina atual
            // take(pageSize) pegar so a quantidade de itens da pagina

            return new PageList<T>(items, count, pageNumber, pageSize); 
        }
        
    }
}
