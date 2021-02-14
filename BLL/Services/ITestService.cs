using System;
using System.IO;
using System.Threading.Tasks;
using DLL.Model;
using DLL.UnitOfWork;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace BLL.Services
{
    public interface ITestService
    {
        Task SaveData();
        Task<byte[]> Reports();
    }

    public class TestService : ITestService
    {
        private int _maxColumn = 3;
        private Document _document;
        private Font _fontStyle;
        PdfPTable _pdfPTable = new PdfPTable(3);
        private PdfPCell _pdfPCell;
        MemoryStream _memoryStream= new MemoryStream();
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;


        public TestService(IUnitOfWork unitOfWork,UserManager<AppUser> userManager,RoleManager<AppRole> roleManager, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
        }
        public async Task SaveData()
        {
            var user = new AppUser
             {
                 UserName = "teacher@gmail.com",
                Email ="teacher@gmail.com"
             };
             var result = await _userManager.CreateAsync(user, "Leads@1234");
             if (result.Succeeded)
             {
                 var role = await _roleManager.FindByNameAsync("teacher");
                 if (role == null)
                 {
                     await _roleManager.CreateAsync(new AppRole
                     {
                         Name = "teacher"
                     });
                    
                     await _userManager.AddToRoleAsync(user, "teacher");
                   
                 }
                 if (role == null)
                 {
                     await _roleManager.CreateAsync(new AppRole
                     {
                         Name = "staff"
                     });
                    
                     await _userManager.AddToRoleAsync(user, "staff");
                   
                 }
             }
            return;
        }

        public async Task<byte[]> Reports()
        {
            _document = new Document(PageSize.A5);
            _document.SetMargins(5f, 5f, 20f, 5f);
            _pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter docWriter = PdfWriter.GetInstance(_document,_memoryStream);
            _document.Open();
            float [] sizes = new float[_maxColumn];
            for (int i = 0; i < _maxColumn; i++)
            {
                sizes[i] = 100;
            }
            _pdfPTable.SetWidths(sizes);
            ReportHeader();
            EmptyRow(2);
            ReportBody();
            _pdfPTable.HeaderRows = 2;
            _document.AddAuthor("Riad");
            _document.AddTitle("Hello World");
            _document.Add(_pdfPTable);
            _document.Close();
            return _memoryStream.ToArray();


        }

        private void ReportHeader()
        {
           _pdfPCell= new PdfPCell(AddLogo());
           _pdfPCell.Colspan=1;
           _pdfPCell.Border = 0;
           _pdfPTable.AddCell(_pdfPCell);
           _pdfPCell= new PdfPCell(setPageTitle());
           _pdfPCell.Colspan=_maxColumn;
           _pdfPCell.Border = 0;
           _pdfPTable.AddCell(_pdfPCell);
           _pdfPTable.CompleteRow();

           
        }

        private PdfPTable setPageTitle()
        {
            int maxClm = 3;
            PdfPTable pdfPTable= new PdfPTable(maxClm);
            _fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Student Management System", _fontStyle));
            _pdfPCell.Colspan = maxClm;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfPCell);
            pdfPTable.CompleteRow();
            
            _fontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Date"+ DateTime.Now.ToString("yy-MM-dd"), _fontStyle));
            _pdfPCell.Colspan = maxClm;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfPCell);
            pdfPTable.CompleteRow();
            return pdfPTable;
        }

        private void EmptyRow(int nCount)
        {

            for (var i = 0; i < nCount; i++)
            {
                _pdfPCell = new PdfPCell(new Phrase(""));
                _pdfPCell.Colspan = 1;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.Border = 0;
                _pdfPCell.ExtraParagraphSpace = 10;
                _pdfPTable.AddCell(_pdfPCell);
                _pdfPTable.CompleteRow();
            }
           
        }

        private void ReportBody()
        {
            var fontstyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            _fontStyle= FontFactory.GetFont("Thoma",9f,0);
            
            _pdfPCell= new PdfPCell(new Phrase("ID",fontstyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor= BaseColor.Gray;
            _pdfPTable.AddCell(_pdfPCell);
            
            _pdfPCell= new PdfPCell(new Phrase("Name",fontstyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor= BaseColor.Gray;
            _pdfPTable.AddCell(_pdfPCell);
            
            _pdfPCell= new PdfPCell(new Phrase("Address",fontstyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor= BaseColor.Gray;
            _pdfPTable.AddCell(_pdfPCell);
            
            _pdfPTable.CompleteRow();

            for (var i = 0; i < 7; i++)
            {
                _pdfPCell= new PdfPCell(new Phrase("ID",_fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor= BaseColor.Gray;
                _pdfPTable.AddCell(_pdfPCell);
            
                _pdfPCell= new PdfPCell(new Phrase("Name",_fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor= BaseColor.Gray;
                _pdfPTable.AddCell(_pdfPCell);
            
                _pdfPCell= new PdfPCell(new Phrase("Address",_fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor= BaseColor.Gray;
                _pdfPTable.AddCell(_pdfPCell);
                _pdfPTable.CompleteRow();
            }


            
            
            
            

        }

        private PdfPTable AddLogo()
        {
            int maxClm =1;
            PdfPTable pdfPTable= new PdfPTable(maxClm);
            string path = _hostEnvironment.WebRootPath + "/Images";
            string imgCombine = Path.Combine(path, "test1.jpg");
            Image image = Image.GetInstance(imgCombine);
            _pdfPCell= new PdfPCell(image);
            _pdfPCell.Colspan = maxClm;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfPCell);
            pdfPTable.CompleteRow();

            return pdfPTable;

        }
    }
}