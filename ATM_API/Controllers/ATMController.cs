using atmBL;
using atmDL;
using Microsoft.AspNetCore.Mvc;

namespace ATM_API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class ATMController : Controller
    {
        SqlDbData _SqlDbData;
        public ATMController()
        {
            _SqlDbData = new SqlDbData();
        }

        [HttpGet("get-users")]
        public IEnumerable<ATM_API.EWallet> GetUsers()
        {
            var ewUsers = _SqlDbData.GetEW();

            List<ATM_API.EWallet> users = new List<EWallet>();

            foreach (var item in ewUsers)
            {
                users.Add(new ATM_API.EWallet { Name = item.name, EWPin = item.EWPin });
            }

            return users;
        }

        [HttpPost("add-user")]
        public JsonResult AddUser(EWallet request)
        {
            var result = _SqlDbData.AddUser(request.Name, request.EWPin);

            return new JsonResult(result);
        }

        [HttpDelete("delete-user")]
        public JsonResult DelUser(EWallet request)
        {
            var result = _SqlDbData.DelUser(request.EWPin);

            return new JsonResult(result);
        }

        [HttpPatch("update-ewallet")]
        public JsonResult UpdateUser(EWallet request)
        {
            var isUpdated = _SqlDbData.TransacDB(request.Money, request.EWPin);

            return new JsonResult(new { Success = isUpdated });
        }

    }
}
