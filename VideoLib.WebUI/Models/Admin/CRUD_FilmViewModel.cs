﻿using System;
using System.ComponentModel.DataAnnotations;

namespace VideoLib.WebUI.Models.Admin
{
    public class CRUD_FilmViewModel
    {
        public string Title { get; set; }
        public int Id { get; set;}
        [Display(Name="Название")]
        [Required]
        public string Name { get; set; }
        
        public GenreViewModel Genre { get; set; }
        public CountryViewModel Country { get; set; }
        public CompanyViewModel Company { get; set; }
        [Display(Name = "Описание")]
        public string Describtion { get; set; }

        [Display(Name = "Дата выхода")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
            ApplyFormatInEditMode = true)]
        public System.DateTime? ReleaseDate { get; set; }
        

        [Display(Name = "Продолжительность")]
        [DisplayFormat(DataFormatString = "{0:h\\:mm\\:ss}",
            ApplyFormatInEditMode = true)]
        public TimeSpan? TimeDuration { get; set; }
        [Display(Name = "К-во скачиваний")]        
        public int DownloadNumber { get; set; }
        [Display(Name = "Режисер")]    
        public string Director { get; set; }
        [Display(Name = "Продюсер")]
        public string Producer { get; set; }
        [Display(Name = "Оператор")]
        public string @Operator { get; set; }
        [Display(Name = "Композитор")]
        public string Composer { get; set; }
        [Display(Name = "Художник")]
        public string Painter { get; set; }
        [Display(Name = "Ссылка на скачивание")]
        [Required]
        public string DownloadUrl { get; set; }
        [Display(Name = "Ссылка на картинку")]
        [Required]
        public string ImageSmallUrl { get; set; }
        public string ImageBigUrl { get; set; }

    }
}