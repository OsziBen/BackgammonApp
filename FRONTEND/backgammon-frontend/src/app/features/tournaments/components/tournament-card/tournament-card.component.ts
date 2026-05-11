import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { TournamentBaseResponse } from '../../models/api/responses/tournament-base.response';
import { CommonModule } from '@angular/common';
import { AppRoutes } from '../../../../shared/constants/app-routes.constants';

@Component({
  selector: 'app-tournament-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tournament-card.component.html',
  styleUrls: ['./tournament-card.component.css'],
})
export class TournamentCardComponent {
  @Input() tournament!: TournamentBaseResponse;

  @Output() join = new EventEmitter<string>();

  constructor(private router: Router) {}

  onJoin() {
    this.join.emit(this.tournament.id);
  }

  onView() {
    this.router.navigate([AppRoutes.tournaments, this.tournament.id]);
  }

  // opcionális: státusz alapú logika
  canJoin(): boolean {
    return this.tournament.status === 'Open';
  }
}
