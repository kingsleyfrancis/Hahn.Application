import { WebAPI } from './web-api';
import { EventAggregator } from 'aurelia-event-aggregator';
import { inject } from 'aurelia-dependency-injection';
import {ApplicantUpdated, ApplicantViewed} from './messages';
import { areEqual } from './utility';
import { Router } from 'aurelia-router';
import { Applicant } from './applicant';

@inject(WebAPI, EventAggregator, Router)
export class ApplicantDetail {
  routeConfig;
  applicant: Applicant;

  constructor(private api: WebAPI, private ea: EventAggregator, private router: Router) {

  }

  activate(params, routeConfig) {
    this.routeConfig = routeConfig;

    return this.api.getApplicant(params.id).then(applicant => {
      this.applicant = <Applicant>applicant;

      let fullname = `${this.applicant.familyName} ${this.applicant.name}`;
      this.routeConfig.navModel.setTitle(fullname);
      this.ea.publish(new ApplicantViewed(this.applicant));
    });
  }

  toggleHired(applicant: Applicant){
    if(applicant.hired){
      applicant.hired = false;
    } else{
      applicant.hired = true;
    }

    this.api.updateApplicant(applicant).then(applicant => {
      if(applicant.errors){
        alert('Updating applicant failed');
      }else{
        this.applicant = applicant;
        alert('Applicant updated successfully');
      }
    });
  }

  deleteApplicant(applicant: Applicant) {
    let result = confirm('Are you sure you want to delete applicant?');

    if(result){
      this.api.deleteApplicant(applicant.id).then((response) => {
        if(response.errors){
          alert('Deleting applicants failed.');
        }else{
          this.router.navigateToRoute("/applicants");
        }
      });
    }   
  }

  get canSave(){
    return this.applicant.name && 
            this.applicant.familyName  &&
            this.applicant.emailAddress &&
            this.applicant.age &&
            this.applicant.countryOfOrigin &&            
            !this.api.isRequesting;
  }

}
