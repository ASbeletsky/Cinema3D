//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VideoLib.Domian.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Film
    {
        public Film()
        {
            this.download = new HashSet<Download>();
            this.favoritefilms = new HashSet<FavoriteFilm>();
            this.comment = new HashSet<Comment>();
            this.rating = new HashSet<Rating>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> AdditionDate { get; set; }
        public string DownloadUrl { get; set; }
        public string ImageSmallUrl { get; set; }
        public string ImageBigUrl { get; set; }
        public float RatingValue { get; set; }
    
        public virtual Desctiption desctiption { get; set; }
        public virtual ProducerStaff producerstaff { get; set; }
        public virtual ICollection<Download> download { get; set; }
        public virtual ICollection<FavoriteFilm> favoritefilms { get; set; }
        public virtual ICollection<Comment> comment { get; set; }
        public virtual ICollection<Rating> rating { get; set; }
    }
}
