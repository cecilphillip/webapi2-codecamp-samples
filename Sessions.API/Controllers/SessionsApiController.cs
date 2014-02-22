﻿using Sessions.API.HttpActions;
using Sessions.Data;
using Sessions.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sessions.API.Controllers
{   
    [RoutePrefix("api/sessions")]
    public class SessionsApiController : ApiController
    {
        private readonly ICodeCampRepository _repo;

        public SessionsApiController(ICodeCampRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllSessions()
        {
            var sessions = _repo.GetSessions();
            if (sessions.Any())
                return Ok(sessions);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("groupByTime")]
        public IHttpActionResult GetSessionsByTime()
        {
            var sessions = _repo.GetSessions();
            if (sessions.Any())
                return Ok(sessions.GroupBy(s => s.StartTime).Select(s => new{Key = s.Key.ToString("HH:mm tt"), Value = s}));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetSession(int id)
        {
            var session = _repo.FindByID(id);
            if (session != null)
                return Ok(session);
            else
                return NotFound();         
        }

        [HttpGet]
        [Route("~/api/presenters")]
        public IHttpActionResult GetPresenters()
        {
            var presenters = _repo.GetSessions().Select(s => s.Presenter).Distinct();
            if (presenters.Any())
                return Ok(presenters);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("~/api/presenters/{name:alpha}/sessions")]
        public IHttpActionResult GetPresenterSessions(string name)
        {
            var presenters = _repo.GetSessions().Where(s => s.Presenter.StartsWith(name,StringComparison.OrdinalIgnoreCase));
            if (presenters.Any())
                return Ok(presenters);
            else
                return NotFound();
        }

    }
}
