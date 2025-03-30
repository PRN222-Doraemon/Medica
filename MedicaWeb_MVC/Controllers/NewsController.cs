using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Specifications.News;
using MedicaWeb_MVC.ViewModels.News;
using MedicaWeb_MVC.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicaWeb_MVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public NewsController(INewsService newsService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _newsService = newsService;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index([FromQuery] NewsParams newsParams)
        {
            ViewData["NewsTypes"] = new SelectList(
                    new List<string> { NewsType.Blog.ToString(), NewsType.Program.ToString() }
                );
            ViewData["Status"] = new SelectList(
               new List<string> { NewsStatus.Active.ToString(), NewsStatus.Disabled.ToString() },
               selectedValue: newsParams.Status?.ToString() ?? NewsStatus.Active.ToString());

            newsParams.Status ??= NewsStatus.Active;
            var spec = new NewsSpecification(newsParams);
            var news = await _newsService.GetNewsAsync(spec);
            var countSpec = new NewsSpecification(newsParams, false);


            var totalNews = (await _newsService.GetNewsAsync(countSpec)).Count();

            var model = new ListVM<NewsVM>()
            {
                Items = _mapper.Map<List<NewsVM>>(news),
                PagingInfo = new PagingVM { CurrentPage = newsParams.Page, TotalItems = totalNews },
                SearchValue = new SearchbarVM { Controller = "News", Action = "Index", SearchText = newsParams.Search }
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var spec = new NewsSpecification(id: id);
            var news = await _newsService.GetNewsWithSpec(spec);

            if (news == null)
            {
                return NotFound();
            }

            var model = new NewsVM
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                ImageUrl = news.ImageUrl,
                NewsType = news.NewsType,
                Status = news.Status,
                CreatedAt = news.CreatedAt,
                UpdatedAt = news.UpdatedAt,
            };

            return View(model);
        }

        [HttpGet]
        [ActionName("Upsert")]
        [Authorize(Roles = AppCts.Roles.Employee)]
        public async Task<IActionResult> UpsertAsync(int? id)
        {
            // Create mode
            if (id == null)
            {
                ViewBag.Title = "Create News";
                ViewBag.NewsTypes = Enum.GetValues<NewsType>()
                    .Cast<NewsType>()
                    .Select(s => new SelectListItem
                    {
                        Value = ((int)s).ToString(),
                        Text = s.ToString()
                    });
                return View(new NewsUpsertVM());
            }

            var news = await _newsService.GetNewsWithSpec(new NewsSpecification(id.Value));
            if (news == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<NewsUpsertVM>(news);
            ViewBag.Title = "Update News";
            ViewBag.NewsTypes = Enum.GetValues<NewsType>()
                .Cast<NewsType>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString()
                });
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AppCts.Roles.Employee)]
        public async Task<IActionResult> Upsert(NewsUpsertVM newsVM)
        {
            if (ModelState.IsValid)
            {
                if (newsVM.Id == 0)
                {
                    newsVM.ImageUrl = await _cloudinaryService.UploadAsync(newsVM.ImageFile);
                    var newsToBeAdded = _mapper.Map<News>(newsVM);
                    await _newsService.CreateNewsAsync(newsToBeAdded);

                    TempData["success"] = "Successfully created the article!";
                }
                else
                {
                    var newsToBeUpdated = await _newsService.GetNewsWithSpec(new NewsSpecification(newsVM.Id));
                    if (newsToBeUpdated == null)
                    {
                        TempData["error"] = "News not found";
                        return NotFound();
                    }
                    _mapper.Map(newsVM, newsToBeUpdated);


                    if (newsVM.ImageFile != null)
                    {
                        await _cloudinaryService.DeleteImageAsync(newsToBeUpdated.ImageUrl);
                        newsToBeUpdated.ImageUrl = await _cloudinaryService.UploadAsync(newsVM.ImageFile);
                    }
                    newsToBeUpdated.UpdatedAt = DateTime.Now;

                    TempData["success"] = "Successfully updated the article!";

                    await _newsService.UpdateNewsAsync(newsToBeUpdated);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = AppCts.Roles.Employee)]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _newsService.GetNewsWithSpec(new NewsSpecification(id));
            if (news == null)
            {
                TempData["error"] = "News not found";
                return NotFound();
            }
            news.Status = NewsStatus.Disabled;
            await _newsService.UpdateNewsAsync(news);
            TempData["success"] = "Successfully disabled the article!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = AppCts.Roles.Employee)]
        public async Task<IActionResult> Enable(int id)
        {
            var news = await _newsService.GetNewsWithSpec(new NewsSpecification(id));
            if (news == null)
            {
                TempData["error"] = "News not found";
                return NotFound();
            }
            news.Status = NewsStatus.Active;
            await _newsService.UpdateNewsAsync(news);
            TempData["success"] = "Successfully enabled the article!";
            return RedirectToAction("Index");
        }
    }

    // // Helper extension method for combining expressions
    // public static class ExpressionExtensions
    // {
    //     public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    //     {
    //         var param = Expression.Parameter(typeof(T), "x");
    //         var body = Expression.AndAlso(
    //             Expression.Invoke(left, param),
    //             Expression.Invoke(right, param)
    //         );
    //         return Expression.Lambda<Func<T, bool>>(body, param);
    //     }
    // }
}