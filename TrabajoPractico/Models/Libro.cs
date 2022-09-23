namespace TrabajoPractico.Models
{
    public class Libro
    {
        public int id { set; get; }
        public string titulo { set; get; }
        public string resumen { set; get; }
        public string foto { set; get; }
        public DateTime fechaPublicacion { set; get; }
        public Genero genero { set; get; }
        public Autor autor { set; get; }
        public int generoid { set; get; }
        public int autorid { set; get; }
    }
}
