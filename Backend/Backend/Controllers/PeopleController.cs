using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Backend.Controllers;
    [Route("api/[controller]")]
    [ApiController]

    public class PeopleController : ControllerBase
    {
        //class düzeyindeki private field (_ kullanımı)
        // POST /api/people {body}
        // GET /api/people
        // GET /api/people/2
        // PUT /api/people/2 {body}
        // DELETE /api/people/2
        private readonly AppDbContext _context;

        public PeopleController(AppDbContext context)//constructor
        {
            //private ile parametreyi ayırmak için _
            _context = context;
        }

        [HttpPost]// POST /api/people
        /*
        Task ise bir işi arka plana atıp, o bitene kadar ana programın kilitlenmesini engelleyen bir yapıdır (Buna Asynchronous / Asenkron denir).

        Task çalıştıran metodun başına async, o task'ın sonucunu beklediğin yere await yazarsın.
        */
        public async Task<IActionResult> AddPerson(Person person)
        {
            try{
                _context.People.Add(person); 
            
                await _context.SaveChangesAsync();// await bir görev tamamlanana kadar beklemeyi sağlar
                /*
                    Bu satır şu anlama gelir:
                    _context.People.Add(person) işlemi yapılmaya başlanıyor.
                    Bu işlem tamamlanmadan await o noktada bekliyor.
                    İşlem bittikten sonra createdPerson değişkenine sonuç atanıyor.
                */

                return CreatedAtRoute("GetPerson", new{id = person.Id}, person); //201 created status code + the location of tehb resource (http://localhost:5295/api/people/{id}) + person object in the response body

            }catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message); //500 internal server error + message in the response body
                
            }
        }

        [HttpGet]// GET /api/people
        public async Task<IActionResult> GetPeople()
        {
            try{

                var people = await _context.People.ToListAsync();

                return Ok(people); //200 Ok status code + person object in the body

            }catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message); //500 internal server error + message in the response body
                
            }
        }


        [HttpGet("{id:int}", Name="GetPerson")]// GET /api/people/1
        public async Task<IActionResult> GetPerson(int id)
        {
            try{

                var person = await _context.People.FindAsync(id);

                if(person is null)
                {
                    return NotFound(); //404 not found status code
                }

                return Ok(person); //200 Ok status code + person object in the body

            }catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message); //500 internal server error + message in the response body
                
            }
        }


        [HttpPut("{id:int}")]// PUT /api/people
        public async Task<IActionResult> UpdatePerson(int id,[FromBody] Person person)
        {
            try{
                if(id != person.Id )
                {
                    return BadRequest("Id in url and body mismatch"); //400 + message in body
                }


                if(!await _context.People.AnyAsync(p => p.Id == id))
                {
                    return NotFound(); //404
                }
                _context.People.Update(person); 
                await _context.SaveChangesAsync();
                return NoContent(); //204 status code

            }catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message); //500 internal server error + message in the response body
                
            }
        }

        [HttpDelete("{id:int}")]// Delete /api/people
        public async Task<IActionResult> DeletePerson(int id)
        {
            try{
                var person = await _context.People.FindAsync(id);

                if(person is null)
                {
                    return NotFound(); //404 not found status code
                }

                _context.People.Remove(person); 
                await _context.SaveChangesAsync();
                return NoContent(); //204 status code

            }catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message); //500 internal server error + message in the response body
                
            }
        }
    }

    // server app: http://localhost:5173 (frontend)
    // client app (js app): http://localhost:5295 =>CORS Request: 

    /*
    [ Tarayıcı (Browser) ]
        │
        │ 1. Kullanıcı http://localhost:3000 adresine girer.
        │    Client App (React ekrandaki butonları) yükler.
        ▼
    [ Client App (Port 3000) ]
        │
        │ 2. Butona basılınca C# API'ye HTTP Request atar:
        │    "GET http://localhost:5295/api/people"
        │
        │    TARAYICI DURUR!
        │    "Dur bakalım! Sen 3000 portundasın ama 5295 portuna
        │     istek atıyorsun. Bu farklı bir orijin (CORS)!"
        │    .NET API'de CORS izni var mı diye bakar.
        ▼
    [ Server App (Port 5295) ]
        │
        │ 3. CORS izni varsa isteği kabul eder.
        │    Veritabanına (SQLite) sorgu atar, kişileri alır.
        │    Cevap olarak JSON döndürür.
        ▼
    [ Client App (Port 3000) ]
        │
        │ 4. Gelen JSON verisini alır ve ekranda tablo olarak gösterir.
        
    */