import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CreateTournamentRequest } from '../../models/api/requests/create-tournament.request';

export type TournamentType = 'Swiss' | 'SingleElimination' | 'RoundRobin';
export type TournamentVisibility = 'Public' | 'Private';

@Component({
  selector: 'app-tournament-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './tournament-form.component.html',
  styleUrls: ['./tournament-form.component.css'],
})
export class TournamentFormComponent {
  @Output() submitted = new EventEmitter<CreateTournamentRequest>();

  typeOptions: TournamentType[] = ['Swiss', 'SingleElimination', 'RoundRobin'];

  visibilityOptions: TournamentVisibility[] = ['Public', 'Private'];

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

  submit() {
    this.submitted.emit(this.model);
  }
}
