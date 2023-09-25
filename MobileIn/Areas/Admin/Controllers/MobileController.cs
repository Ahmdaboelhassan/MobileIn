using Business.IRepository;
using Business.SD;
using Business.ViewModels;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MobileIn.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{SD.Admin},{SD.SuperAdmin}")]

public class MobileController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _wwwRoot;
    private readonly MobileViewModel mobileViewModel;

    public MobileController(IUnitOfWork unitOfWork , IWebHostEnvironment wwwRoot)
    {
        _unitOfWork = unitOfWork;
        _wwwRoot = wwwRoot;
        mobileViewModel = new MobileViewModel()
        {
            Companies = _unitOfWork.Companies.GetAll(),
            Processors = _unitOfWork.Processors.GetAll(new string[] { SD.COMPANY })
        };

    }

    public IActionResult MobileGET(int? id = null)
    {
        if(id is 0 || id is null)
        {
            mobileViewModel.Mobile = new Mobile();
        }
        else
        {
          Mobile? oldMobile = _unitOfWork.Mobiles.GetFirstOrDefault(m => m.id == id);
            if (oldMobile is null)
                return NotFound();
            mobileViewModel.Mobile = oldMobile;

        }
        return View(mobileViewModel);
    }


    private bool Checkimage(IFormFile image)
    {
        //check Extistion and Length
        List<string> AllowedExtinstions = new List<string> { ".png", ".jpg" };
        if (!AllowedExtinstions.Contains(Path.GetExtension(image.FileName.ToLower())))
        {
            ModelState.AddModelError("image", "Extistion Not Allowed!!");
            return false;
        }
        if (image.Length > 2_097_152)
        {
            ModelState.AddModelError("image", "Length Not Allowed!! , The Max Length Is 2MB");
            return false;
        }
        return true;

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult InsertMobile(Mobile mobile)
    {
        mobileViewModel.Mobile = mobile;

        // Handle Image
        var files = Request.Form.Files;
        if (!files.Any())
        {
            ModelState.AddModelError("image", "Mobile Image Can Not Be Empty ");
            return View("MobileGET", mobileViewModel);
        }
        IFormFile image = files.FirstOrDefault();

        if (!Checkimage(image))
        {
            return View("MobileGET", mobileViewModel);
        }

        string randomName = Guid.NewGuid().ToString();
        string extension = Path.GetExtension(image.FileName);
        string fullPath = Path.Combine(_wwwRoot.WebRootPath, "Images", "Mobile", randomName + extension);

        using(var file = new FileStream(fullPath , FileMode.Create))
        {
            image.CopyTo(file);
        }
        mobile.photoURL = Path.Combine("\\Images", "Mobile", randomName + extension);

        if (!ModelState.IsValid)
        {
            return View("MobileGET", mobileViewModel);

        }
        _unitOfWork.Mobiles.Add(mobile);
        _unitOfWork.SaveChanges();
        
        TempData["Success"] = "Mobile Inserted Successfully";

        return RedirectToAction("Mobiles", "Dashbourd", new { active = "Mobiles" });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateMobile(Mobile mobile)
    {
        Mobile oldMobile = _unitOfWork.Mobiles.GetFirstOrDefault(m => m.id == mobile.id);
        mobileViewModel.Mobile = mobile;

        // Handle Image
        var files = Request.Form.Files;
        if (!files.Any())
        {
            mobile.photoURL = oldMobile.photoURL;
        }
        else
        {
            string oldImage = _wwwRoot.WebRootPath + oldMobile.photoURL;
            
            if(System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            };
            IFormFile image = files.FirstOrDefault();

            if (!Checkimage(image))
            {
                return View("MobileGET", mobileViewModel);
            }

            string randomName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(image.FileName);
            string fullPath = Path.Combine(_wwwRoot.WebRootPath, "Images", "Mobile", randomName + extension);

            using (var file = new FileStream(fullPath, FileMode.Create))
            {
                image.CopyTo(file);
            }
            mobile.photoURL = Path.Combine("\\Images", "Mobile", randomName + extension);

        }
        if (!ModelState.IsValid)
        {
            return View("MobileGET", mobileViewModel);
        }

        oldMobile.RAM = mobile.RAM;
        oldMobile.price = mobile.price;
        oldMobile.name = mobile.name;
        oldMobile.companyId = mobile.companyId;
        oldMobile.description = mobile.description;
        oldMobile.processorId = mobile.processorId;
        oldMobile.size = mobile.size;
        oldMobile.yearRelease = mobile.yearRelease;
        oldMobile.photoURL = mobile.photoURL;

        _unitOfWork.Mobiles.Update(oldMobile);
        _unitOfWork.SaveChanges();

        TempData["Success"] = "Mobile Updated Successfully";

        return RedirectToAction("Mobiles" , "Dashbourd" ,new {active = "Mobiles"});
    }


    [HttpDelete]
    public IActionResult MobileDelete(int id)
    {
        Mobile oldMobile = _unitOfWork.Mobiles.GetFirstOrDefault(x => x.id == id);
        if (oldMobile == null)
            return NotFound();

        // Delete Image From The Server 
        string oldImage = _wwwRoot.WebRootPath + oldMobile.photoURL;
        if (System.IO.File.Exists(oldImage))
        {
            System.IO.File.Delete(oldImage);
        };
        _unitOfWork.Mobiles.Delete(oldMobile);
        _unitOfWork.SaveChanges();

        TempData["Success"] = "Mobile Deleted Successfully";

        return Ok();
    }


}

