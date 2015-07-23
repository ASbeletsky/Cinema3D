using System;
using System.ComponentModel.DataAnnotations;

namespace VideoLib.WebUI.Models.Admin
{
    public class CRUD_FilmViewModel
    {
        public string Action { get; set; }
        public string Style { get; set; }
        public string Title { get; set; }
        public GenreViewModel Genre { get; set; }
        public CompanyViewModel Company { get; set; }
        public CountryViewModel Country { get; set; }
//------------------------------------------------------
        public int Id { get; set;}
        
        [Required]
        [Display(Name = "Название")]
        [MinLength(2)]
        public string Name { get; set; }
//--------------------------------------------------------       
        [Display(Name = "Описание")]
        public string Describtion { get; set; }
//--------------------------------------------------------      
        [Required]
        [Display(Name = "Дата выхода")]

        public System.DateTime? ReleaseDate { get; set; }
//----------------------------------------------------------        
        [Display(Name = "Продолжительность")]

        public TimeSpan? TimeDuration { get; set; }
//-----------------------------------------------------------
        [Display(Name = "Рейтинг")]
        public float Rating { get; set; }
//-----------------------------------------------------------
        [Display(Name = "К-во скачиваний")]        
        public int DownloadNumber { get; set; }
//-----------------------------------------------------------
        public float DownloadRating { get; set; }
//-----------------------------------------------------------
        [Display(Name = "К-во добавлений в избранное")]
        public int AddionFavorite{ get; set; }
//-----------------------------------------------------------
        public float AddionFavoriteRating{ get; set; }
//-----------------------------------------------------------
        [Display(Name = "Режисер")]    
        public string Director { get; set; }
//-----------------------------------------------------------
        [Display(Name = "Продюсер")]
        public string Producer { get; set; }
//-----------------------------------------------------------
        [Display(Name = "Оператор")]
        public string @Operator { get; set; }
//-----------------------------------------------------------
        [Display(Name = "Композитор")]
        public string Composer { get; set; }
//-----------------------------------------------------------
        [Display(Name = "Художник")]
        public string Painter { get; set; }
//-----------------------------------------------------------
        [Display(Name = "Ссылка на скачивание")]
        [Required]
        public string DownloadUrl { get; set; }
//-----------------------------------------------------------
        [Display(Name = "В перечне")]
        [Required]
        public string ImageSmallUrl { get; set; }
//-----------------------------------------------------------
        [Display(Name = "Для постера")]
        [Required]
        public string ImageBigUrl { get; set;} 
//-----------------------------------------------------------

    }
}