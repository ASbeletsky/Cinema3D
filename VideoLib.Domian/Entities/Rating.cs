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
    
    public partial class Rating
    {
        public sbyte RatingValue { get; set; }
        public string User_Id { get; set; }
        public int Film_Id { get; set; }
        public Nullable<System.DateTime> AdditionTime { get; set; }
    
        public virtual Film film { get; set; }
        public virtual users users { get; set; }
    }
}
