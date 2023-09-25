using Business.IRepository;
using Business.SD;
using Data.Models;
using MobileIn.Areas.Customer.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MobileIn.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize (Roles =$"{SD.Admin},{ SD.SuperAdmin}")]
public class CompanyController : Controller
{
    
    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
   
    public IActionResult CompanyGET(int? id)
    {
        Company Item;
        if (id == 0 || id is null)
        {
            Item = new Company();
        }
        else
        {
            Company? oldCompany = _unitOfWork.Companies.GetFirstOrDefault(c => c.id == id);
            if (oldCompany is null)
                return NotFound();
            
            Item = oldCompany;
        }

        return View(Item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company newCompany)
    {
        if (ModelState.IsValid)
        {
            if(newCompany.id == 0)
            {
                _unitOfWork.Companies.Add(newCompany);
                TempData["Success"] = "Company Added Successfully";
            }
            else
            {
               _unitOfWork.Companies.Update(newCompany);
                  TempData["Success"] = "Company Updated Successfully";
              
            }
            _unitOfWork.SaveChanges();
             return RedirectToAction(nameof(Index),"Dashbourd" );
        }
        return View("CompanyGET", newCompany);
    }

    // Delete 
    [HttpDelete]
    public IActionResult DeleteCompany(int id)
    {
        Company oldCompany = _unitOfWork.Companies.GetFirstOrDefault(c => c.id == id);
        if (oldCompany == null)
            return NotFound();

        Processor? ProcessorWithCompany = _unitOfWork.Processors.GetFirstOrDefault(c => c.companyId == id);
        Mobile? MobileWithCompany = _unitOfWork.Mobiles.GetFirstOrDefault(c => c.companyId == id);
        if(MobileWithCompany is not null || ProcessorWithCompany is not null)
        {
            TempData["Error"] = "This Company Is Asscoited With Processor or Mobile";
            return Ok();

        }

        _unitOfWork.Companies.Delete(oldCompany);
        _unitOfWork.SaveChanges();

        TempData["Success"] = "Company Deleted Successfully";

        return Ok();

    }

}
