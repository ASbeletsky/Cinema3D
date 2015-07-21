using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoLib.Domian.Entities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace VideoLib.WebUI.Models.Admin
{
    public class CompanyViewModel
    {
          private readonly List<Company> _companies;
          public CompanyViewModel() { }

          public CompanyViewModel(List<Company> companies)
          {
              _companies = companies;
          }

          [Display(Name = "Кинокомпания")]
          public int SelectedCompanyId { get; set; }

          public IEnumerable<SelectListItem> CompanyItems
          {
              get
              {
                  var AllCompanies = _companies.Select(g => new SelectListItem
                  {
                      Value = g.Id.ToString(),
                      Text = g.Name
                  });
                  return AllCompanies;
              }
          }
    }
}