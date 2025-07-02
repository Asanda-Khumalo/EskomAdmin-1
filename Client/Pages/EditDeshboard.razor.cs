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
    public partial class EditDeshboard
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

        [Parameter]
        public int TrendNumber { get; set; }

        protected override async Task OnInitializedAsync()
        {
            deshboard = await DeshboardService.GetDeshboardByTrendNumber(trendNumber:TrendNumber);
        }
        protected bool errorVisible;
        protected EskomAdmin.Server.Models.Deshboard.Deshboard deshboard;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await DeshboardService.UpdateDeshboard(trendNumber:TrendNumber, deshboard);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            hasChanges = false;
            canEdit = true;

            deshboard = await DeshboardService.GetDeshboardByTrendNumber(trendNumber:TrendNumber);
        }
    }
}