using CL.Module.ContactList.Application.Commands.AddContact;
using CL.Module.ContactList.Application.Commands.DeleteContact;
using CL.Module.ContactList.Application.Commands.EditContact;
using CL.Module.ContactList.Application.Dtos;
using CL.Module.ContactList.Application.Queries.GetCategories;
using CL.Module.ContactList.Application.Queries.GetContactsQuery;
using CL.Module.ContactList.Application.Requests;
using CL.Shared.Abstractions.Auth;
using CL.Shared.Abstractions.Dispatchers;
using CL.Shared.Infrastructure.Auth;
using CL.Shared.Infrastructure.Controllers;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CL.Module.ContactList.Api.Controllers
{
    public class ContactListController(IDispatcher dispatcher, ICurrentUserProvider currentUserProvider)
        : BaseController(dispatcher, currentUserProvider)
    {
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<ContactDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ContactDto>>> GetContacts(CancellationToken cancellationToken = default)
        {
            var query = new GetContactsQuery();
            var contacts = await dispatcher.QueryAsync(query, cancellationToken);

            return Ok(contacts);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<ContactCategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ContactCategoryDto>>> GetCategories(CancellationToken cancellationToken = default)
        {
            var query = new GetCategoriesQuery();
            var categories = await dispatcher.QueryAsync(query, cancellationToken);

            return Ok(categories);
        }

        [HttpGet("[action]/{contactId}")]
        [ProducesResponseType(typeof(ContactDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ContactDetailsDto>> GetContactDetails([FromRoute] int contactId, CancellationToken cancellationToken = default)
        {
            var query = new GetContactDetailsQuery(contactId);
            var contactDetails = await dispatcher.QueryAsync(query, cancellationToken);

            return Ok(contactDetails);
        }

        [AuthorizeRoles(Role.User)]
        [HttpPut("[action]")]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ContactDto>> AddContact([FromBody] AddContactRequest request, CancellationToken cancellationToken = default)
        {
            var command = new AddContactCommand(
                request.Name,
                request.Surname,
                request.Email,
                request.CategoryId,
                request.SubCategory,
                request.Phone,
                request.DateOfBirth);

            var result = await dispatcher.SendAsync<AddContactCommand, Result<ContactDto>>(command, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Conflict(result);
        }

        [AuthorizeRoles(Role.User)]
        [HttpPatch("[action]")]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ContactDto>> EditContact([FromBody] EditContactRequest request, CancellationToken cancellationToken = default)
        {
            var command = new EditContactCommand(
                request.ContactId,
                request.Name,
                request.Surname,
                request.Email,
                request.CategoryId,
                request.SubCategory,
                request.Phone,
                request.DateOfBirth);

            var result = await dispatcher.SendAsync<EditContactCommand, Result<ContactDto>>(command, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return Conflict(result);
        }

        [AuthorizeRoles(Role.User)]
        [HttpDelete("[action]/{contactId}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Result>> DeleteContact([FromRoute] int contactId, CancellationToken cancellationToken = default)
        {
            var command = new DeleteContactCommand(contactId);
            var result = await dispatcher.SendAsync<DeleteContactCommand, Result>(command, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return Conflict(result);
        }
    }
}
