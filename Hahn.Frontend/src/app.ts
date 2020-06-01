import { WebAPI } from './web-api';
import { inject } from 'aurelia-dependency-injection';
import { Router, RouterConfiguration } from 'aurelia-router';
import { PLATFORM } from 'aurelia-pal';

@inject(WebAPI)
export class App {
  title: string = "Hahn App";
  router: Router;

  constructor(public api: WebAPI){}

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'Applicants';
    config.options.pushState = true;
    config.options.root = '/';
    config.map([
      {route: '', moduleId: PLATFORM.moduleName('create-applicant'), name: 'create', title:"Create Applicant"},
      {route: 'applicants/:id', moduleId: PLATFORM.moduleName('applicant-detail'), name: 'applicant', title:"View Applicant" },
      {route: 'applicants', moduleId: PLATFORM.moduleName('applicant-list'), name: 'applicants', title: "List Applicant"}
    ]);

    this.router = router;
  }
}
