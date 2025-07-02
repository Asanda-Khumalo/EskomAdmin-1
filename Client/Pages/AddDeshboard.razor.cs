using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace EskomAdmin.Client.Pages
{
    public partial class AddDeshboard
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public DeshboardService DeshboardService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            deshboard = new EskomAdmin.Server.Models.Deshboard.Deshboard();
        }
        protected bool errorVisible;
        protected EskomAdmin.Server.Models.Deshboard.Deshboard deshboard;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await DeshboardService.CreateDeshboard(deshboard);
                DialogService.Close(deshboard);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}