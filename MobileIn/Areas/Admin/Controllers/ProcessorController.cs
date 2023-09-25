using Business.IRepository;
using Business.SD;
using Business.ViewModels;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace MobileIn.Areas.Admin.Controllers;

[Area("admin")]
[Authorize(Roles = $"{SD.Admin},{SD.SuperAdmin}")]

public class ProcessorController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProcessorController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Add Function
    public IActionResult ProcessorGET(int? id)
    {
        ProcessorViewModel currentProcessor = new ProcessorViewModel();
        currentProcessor.Companies = _unitOfWork.Companies.GetAll();

        if (id is null || id == 0) {

            currentProcessor.processor = new Processor();
        }
        else {
            Processor oldProcessor = _unitOfWork.Processors.GetFirstOrDefault(p => p.id == id);
            if (oldProcessor is null)
                return NotFound();
           
            currentProcessor.processor = oldProcessor;
        }

        return View(currentProcessor);

    }

    private bool Checkimage(IFormFile image , ProcessorViewModel processorViewModel)
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
    public IActionResult InsertProcessor(ProcessorViewModel processorViewModel, IFormFile? image)
    {
        // intilaize Dropdown List 
        processorViewModel.Companies = _unitOfWork.Companies.GetAll();

        //check image 
        if(image is null)
        {
            ModelState.AddModelError("image", "Image Can Not Be Null");
            return View("ProcessorGET", processorViewModel);
        }

        // Hanlde Image 
        using (MemoryStream stream = new MemoryStream())
        {

            image.CopyTo(stream);
            processorViewModel.processor.photo = stream.ToArray();

        }
        //  check extinstion and length
        if (!Checkimage(image, processorViewModel))
        {
            return View("ProcessorGET", processorViewModel);
        }
        
       
        if (!ModelState.IsValid)
            return View("ProcessorGET", processorViewModel);

        Processor newProcessor = new Processor()
        {
            name = processorViewModel.processor.name,
            companyId = processorViewModel.processor.companyId,
            photo = processorViewModel.processor.photo,
        };

        _unitOfWork.Processors.Add(newProcessor);
        _unitOfWork.SaveChanges();
        TempData["Success"] = "Processor Inserted Successfully";

        return RedirectToAction("Processors", "Dashbourd", new { active = "Processors" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateProcessor(ProcessorViewModel processorViewModel, IFormFile? image)
    {
        Processor oldProcessor = _unitOfWork.Processors.GetFirstOrDefault(p => p.id == processorViewModel.processor.id);

        // intilaize Dropdown List 
        processorViewModel.Companies = _unitOfWork.Companies.GetAll();

        if (image is not null)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.CopyTo(stream);
                processorViewModel.processor.photo = stream.ToArray();

            }
            //  check extinstion and length
            if (!Checkimage(image, processorViewModel))
            {
                return View("ProcessorGET", processorViewModel);
            }
        }
        else
        {
            processorViewModel.processor.photo = oldProcessor.photo;
        }
        if(!ModelState.IsValid) {
            return View("ProcessorGET", processorViewModel);
        }

        oldProcessor.name = processorViewModel.processor.name;
        oldProcessor.companyId = processorViewModel.processor.companyId;
        oldProcessor.photo = processorViewModel.processor.photo;

        _unitOfWork.Processors.Update(oldProcessor);
        _unitOfWork.SaveChanges();

        TempData["Success"] = "Processor Updated Successfully";

        return RedirectToAction("Processors", "Dashbourd", new { active = "Processors" });
    }


    [HttpDelete]
    public IActionResult ProcessorDelete(int id)
    {
        Processor oldProcessor = _unitOfWork.Processors.GetFirstOrDefault(p => p.id == id);

        if (oldProcessor == null)
            return NotFound();
      

        Mobile? ProceesorWithMobile = _unitOfWork.Mobiles.GetFirstOrDefault(m => m.processorId == id , new string[] { SD.PROCESSOR });
        if (ProceesorWithMobile is not null)
        {
            TempData["Error"] = "The Processor Is Asscoited With Mobile";
            return Ok();
        }

        _unitOfWork.Processors.Delete(oldProcessor);
        _unitOfWork.SaveChanges();

        TempData["Success"] = "Processor Deleted Successfully";
        return Ok();
    }

}


