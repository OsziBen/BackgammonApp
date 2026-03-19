import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GameSessionSettings } from '../../models/game-session-settings.model';

@Component({
  selector: 'app-create-game-session',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-game-session.component.html',
})
export class CreateGameSessionComponent implements OnInit {
  @Output()
  sessionCreated = new EventEmitter<GameSessionSettings>();

  form: FormGroup;

  constructor(private readonly fb: FormBuilder) {
    this.form = this.fb.group({
      targetPoints: [{ value: 1, disabled: true }],
      doublingCubeEnabled: [{ value: false, disabled: false }],
      clockEnabled: [{ value: false, disabled: true }],
      matchTimePerPlayerInSeconds: [{ value: null, disabled: true }],
      startOfTurnDelayPerPlayerInSeconds: [{ value: null, disabled: true }],
      crawfordRuleEnabled: [{ value: false, disabled: true }],
    });
  }
  ngOnInit(): void {
    const clockEnabledControl = this.form.get('clockEnabled');

    clockEnabledControl?.valueChanges.subscribe((enabled: boolean) => {
      const matchTime = this.form.get('matchTimePerPlayerInSeconds');
      const turnDelay = this.form.get('startOfTurnDelayPerPlayerInSeconds');

      if (enabled) {
        matchTime?.enable();
        turnDelay?.enable();
      } else {
        matchTime?.disable();
        turnDelay?.disable();
      }
    });
  }

  create(): void {
    const settings = this.form.getRawValue() as GameSessionSettings;
    this.sessionCreated.emit(settings);
  }
}
