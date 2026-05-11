import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { RULES_TEMPLATE_API_ROUTES } from '../constants/rules-template-api-routes.constants';
import { RulesTemplateResponse } from '../models/api/responses/rules-template.response';

@Injectable({
  providedIn: 'root',
})
export class RulesTemplatesApiService {
  private readonly rulesTemplatesBaseUrl = `${environment.apiBaseUrl}/${RULES_TEMPLATE_API_ROUTES.BASE}`;

  constructor(private http: HttpClient) {}

  // GET ALL RULES TEMPLATES
  getAllTemplates(): Observable<RulesTemplateResponse[]> {
    return this.http.get<RulesTemplateResponse[]>(this.rulesTemplatesBaseUrl);
  }
}
