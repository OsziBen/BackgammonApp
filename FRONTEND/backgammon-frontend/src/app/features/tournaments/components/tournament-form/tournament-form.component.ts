import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CreateTournamentRequest } from '../../models/api/requests/create-tournament.request';
import { RulesTemplateResponse } from '../../../rules-template/models/api/responses/rules-template.response';
import { RulesTemplatesApiService } from '../../../rules-template/services/rules-templates-api.service';

export type TournamentType = 'Swiss' | 'SingleElimination' | 'RoundRobin';
export type TournamentVisibility = 'Public' | 'Private';

@Component({
  selector: 'app-tournament-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './tournament-form.component.html',
  styleUrls: ['./tournament-form.component.css'],
})
export class TournamentFormComponent implements OnInit {
  @Output() submitted = new EventEmitter<CreateTournamentRequest>();

  typeOptions: TournamentType[] = ['Swiss', 'SingleElimination', 'RoundRobin'];

  visibilityOptions: TournamentVisibility[] = ['Public', 'Private'];

  rulesTemplates: RulesTemplateResponse[] = [];

  model: CreateTournamentRequest = {
    name: '',
    description: '',
    type: 'Swiss',
    visibility: 'Public',
    maxParticipants: 8,
    rulesTemplateId: '',
    startDate: '',
    endDate: '',
    deadline: '',
  };
  constructor(private readonly rulesTemplatesApi: RulesTemplatesApiService) {}

  ngOnInit(): void {
    this.loadRulesTemplates();
  }

  private loadRulesTemplates(): void {
    this.rulesTemplatesApi.getAllTemplates().subscribe({
      next: (data) => {
        this.rulesTemplates = data;

        if (data.length > 0 && !this.model.rulesTemplateId) {
          this.model.rulesTemplateId = data[0].id;
        }
      },
      error: (err) => {
        console.error('Failed to load rules templates', err);
      },
    });
  }

  submit() {
    this.submitted.emit(this.model);
  }
}
