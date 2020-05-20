﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Migrations;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerServices;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerServices, DepartmentService departmentService)
        {
            _sellerServices = sellerServices;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var list = _sellerServices.FindAll();

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var ViewModel = new SellerFormViewModel{ Departments = departments };
            return View(ViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var ViewModel = new SellerFormViewModel { Departments = departments };
                return View(nameof(ViewModel));
            }
            _sellerServices.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete (int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            var obj = _sellerServices.FindById(id.Value);
            if (obj == null)
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerServices.Remove(id);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            var obj = _sellerServices.FindById(id.Value);
            if (obj == null)
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            return View(obj);
        }


        public IActionResult Edit(int ? id)
        {

            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            var obj = _sellerServices.FindById(id.Value);
            if (obj == null)
                return RedirectToAction(nameof(Error), new { message = "Id not found" });

            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel {Seller = obj, Departments = departments };
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var ViewModel = new SellerFormViewModel {Seller= seller, Departments = departments };
                return View(nameof(ViewModel));
            }
            if (id != seller.Id)
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            try
            {
                _sellerServices.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch(NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch(DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}