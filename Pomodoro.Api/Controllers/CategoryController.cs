// <copyright file="CategoryController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Api.Controllers.Base;
using Pomodoro.Dal.Entities;
using Pomodoro.Services;
using Pomodoro.Services.Models;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// Manage category.
    /// </summary>
    public class CategoryController : BaseController<CategoryService, Category, CategoryModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="service">Instance of Category service.</param>
        public CategoryController(CategoryService service)
            : base(service)
        {
        }
    }
}
