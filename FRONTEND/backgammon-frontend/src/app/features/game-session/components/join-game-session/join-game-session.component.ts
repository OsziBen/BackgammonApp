import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { GameSessionStore } from '../../state/game-session.store';
import { ConnectionState } from '../../models/enums/connection-state.enum';

@Component({
  selector: 'app-join-game-session',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './join-game-session.component.html',
})
export class JoinSessionComponent {
  form: FormGroup;

  constructor(
    private readonly fb: FormBuilder,
    public readonly store: GameSessionStore,
  ) {
    this.form = this.fb.group({
      sessionCode: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  @Output() joinSession = new EventEmitter<string>();

  onJoin(): void {
    if (this.form.invalid) {
      return;
    }

    const sessionCode = this.form.value.sessionCode.trim();

    this.joinSession.emit(sessionCode);
  }

  public isLoading(): boolean {
    const state = this.store.connectionState();

    return (
      state === ConnectionState.Connecting ||
      state === ConnectionState.Reconnecting
    );
  }

  public isConnected(): boolean {
    return this.store.connectionState() === ConnectionState.Connected;
  }

  public hasError(): boolean {
    return false;
  }
}
