<template>
	<b-container class="d-flex flex-column align-items-center pt-5 main">
		<h3 class="mb-5">Встречи, самые разные</h3>

		<b-container class="auth-block">
			<b-container>
				<b-form-input
					v-model="signInForm.nickname"
					:state="validateNickname(signInForm.nickname)"
					placeholder="Никнейм"
					trim
					class="mb-3"
					v-on:keyup.enter="signIn"
					id="sign-in-nickname"
				></b-form-input>

				<b-form-input
					v-model="signInForm.password"
					:state="validatePassword(signInForm.password)"
					placeholder="Пароль"
					class="mb-3"
					type="password"
					v-on:keyup.enter="signIn"
					id="sign-in-password"
				></b-form-input>

				<b-button
					variant="outline-primary" block :disabled="!validateSignInForm" v-on:click="signIn"
					id="sign-in-button"
				>
					Войти
				</b-button>
			</b-container>
		</b-container>

		<b-container class="auth-block my-5">
			<b-container>
				<b-form-input
					v-model="signUpForm.nickname"
					:state="validateNickname(signUpForm.nickname)"
					placeholder="Никнейм"
					trim
					class="my-3"
					v-on:keyup.enter="signUp"
					id="sign-up-nickname"
				></b-form-input>

				<b-form-input
					v-model="signUpForm.password"
					:state="validatePassword(signUpForm.password)"
					placeholder="Пароль"
					class="my-3"
					type="password"
					v-on:keyup.enter="signUp"
					id="sign-up-password"
				></b-form-input>

				<b-form-input
					v-model="signUpForm.confirmation"
					:state="validatePassword(signUpForm.confirmation) && signUpForm.password === signUpForm.confirmation"
					placeholder="Подтверждение"
					class="my-3"
					type="password"
					v-on:keyup.enter="signUp"
					id="sign-up-confirmation"
				></b-form-input>

				<b-button
					variant="outline-primary" block
					:disabled="!validateSignUpForm"
					v-on:click="signUp"
					id="sign-up-button"
				>
					Зарегистрироваться
				</b-button>
			</b-container>
		</b-container>
	</b-container>
</template>

<script>
	import {signIn, signUp} from '../services/authentication.service';
	import {setNickname, setToken} from '../tools/token';

	export default {
		name: 'AuthPage',
		data: () => ({
			signInForm: {
				nickname: '',
				password: '',
			},

			signUpForm: {
				nickname: '',
				password: '',
				confirmation: '',
			},
		}),

		methods: {
			validateNickname(nickname) {
				return nickname.length > 0;
			},

			validatePassword(password) {
				return password.length >= 8;
			},

			async signUp() {
				if (!this.validateSignUpForm) {
					return;
				}

				const {data} = await signUp(this.signUpForm.nickname, this.signUpForm.password);
				setToken(data);
				setNickname(this.signUpForm.nickname);

				await this.$router.push({name: 'main-home'});
			},

			async signIn() {
				if (!this.validateSignInForm) {
					return;
				}

				const {data} = await signIn(this.signInForm.nickname, this.signInForm.password);
				setToken(data);
				setNickname(this.signInForm.nickname);
				await this.$router.push({name: 'main-home'});
			},
		},

		computed: {
			validateSignInForm() {
				return this.validateNickname(this.signInForm.nickname)
					&& this.validatePassword(this.signInForm.password);
			},

			validateSignUpForm() {
				return this.validateNickname(this.signUpForm.nickname)
					&& this.validatePassword(this.signUpForm.password)
					&& this.signUpForm.password === this.signUpForm.confirmation;
			},
		},
	};
</script>

<style scoped>
	.auth-block {
		max-width: 500px;
		width: 50%;
		margin: 10px;
	}

	.main {
		margin-top: 40px;
	}
</style>
