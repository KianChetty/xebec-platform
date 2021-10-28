﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using XebecPortal.Shared;

namespace XebecPortal.Client.JobPortalTestEnv.Jobport_3.Pages
{
    public partial class JobPostingForCandidate
    {
        //Show and Hide Element
        private bool IsShown { get; set; } = false;

        private List<Application> applications = new List<Application>();
        private static List<Job> Jobs = new List<Job>();
        public List<JobType> JobTypes { get; set; }

        protected override async Task OnInitializedAsync()
        {



            try
            {
                Jobs = await httpClient.GetFromJsonAsync<List<Job>>("api/Job");
                JobTypes = await httpClient.GetFromJsonAsync<List<JobType>>("api/jobtype");
            }
            catch (Exception ex)
            {
                Jobs = new List<Job>();
                JobTypes = new List<JobType>();
            }

            if (Jobs != null)
            {
                DisplayJobs = Jobs;
                CurrentJob2 = Jobs[0];
            }

            await base.OnInitializedAsync();

        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jsr.InvokeVoidAsync("initialize"); //Calls the initialize function from the javascript file.
            }
        }

        public Job EditJob { get; set; } = new Job();

        public List<Job> DisplayJobs { get; set; }

        public Job CurrentJob2 { get; set; } = new Job();

        private async Task ViewJob(Job JobToView, int userId)
        {


            try
            {
                applications = await httpClient.GetFromJsonAsync<List<Application>>("api/Application");
            }
            catch (Exception ex)
            {
                applications = new List<Application>();
            }

            if (applications.Count(x => x.JobId == JobToView.Id && x.AppUserId == userId) >= 1)
                IsShown = true;
            else
                IsShown = false;

            CurrentJob2 = JobToView;
        }

        private static List<Job> SearchResults = Jobs;

        // Apply Function
        private async Task Apply()
        {
            await httpClient.PostAsJsonAsync("api/Application", CurrentJob2);
            await jsr.InvokeVoidAsync("alert", "You Have Applied Successfully");
            IsShown = !IsShown;
        }

        private bool IsClicked = false;

        private int ReturnedJobId = 0;

        private Job CurrentJob = null;

        //Method for when a summary box is clicked. Uses the JobPostingTest.js in the js folder inside the wwwroot.
        private async Task On()
        {
            IsClicked = true;
            ReturnedJobId = Int32.Parse(await jsr.InvokeAsync<string>("show")); //Calls the show function from the javascript file.

            CurrentJob = SearchResults.FirstOrDefault(q => q.Id == ReturnedJobId);
        }




        #region Searching and Filtering

        private string SearchTerm { get; set; } = String.Empty;
        public string JobFilter { get; set; } = String.Empty;
        private bool jobFilterApplied = false;
        private List<Job> SearchedJobs { get; set; } = new List<Job>();

        private void onValChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            JobFilter = args.Value.ToString();
            jobFilterApplied = true;
            SearchEvent();
        }

        private void Clear()
        {
            jobFilterApplied = false;
            JobFilter = string.Empty;
            SearchEvent();
        }

        private void RealSearch()
        {

            SearchEvent();

        }

        private async Task<List<Job>> SearchEvent()
        {

            try
            {

                SearchedJobs = await httpClient.GetFromJsonAsync<List<Job>>($"api/jobtest/?searchQuery={SearchTerm}&jobtypeQuery={JobFilter}");
            }
            catch (Exception ex)
            {
                SearchedJobs = new List<Job>();
            }

            if (SearchedJobs.Count > 0)
            {
                CurrentJob2 = SearchedJobs[0];
            }
            DisplayJobs = SearchedJobs;
            InvokeAsync(StateHasChanged);
            return Jobs;
        }

        #endregion
    }
}