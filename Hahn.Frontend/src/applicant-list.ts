import { Applicant } from './applicant';
import { inject } from 'aurelia-dependency-injection';
import { EventAggregator } from 'aurelia-event-aggregator';
import { WebAPI } from './web-api';
import {ApplicantUpdated, ApplicantViewed, ApplicantCreated} from './messages';
import { View } from 'aurelia-framework';

@inject(WebAPI, EventAggregator)
export class ApplicantList {
  applicants : Applicant[] = [];
  selectedId = 0;

  constructor(private api: WebAPI, ea: EventAggregator) {
    ea.subscribe(ApplicantViewed, msg => this.select(msg.applicant));
    ea.subscribe(ApplicantUpdated, msg => {
      let id = msg.applicant.id;
      let found = this.applicants.find(x => x.id == id);
      let index = this.applicants.indexOf(found);
      Object.assign(found, msg.applicant);

      if(index){
        this.applicants.splice(index, 1, found);
      }
    });
    ea.subscribe(ApplicantCreated, msg => {
      let applicant = msg.applicant;
      this.applicants.push(applicant);
    });
  }

  created(owningView: View, myView: View) {
    this.api.getApplicants().then(applicants => {
      this.applicants = applicants;
      console.log('Applicants: ', this.applicants);
    });
  }

  select(applicant){
    this.selectedId = applicant.id;
    return true;
  }
}
