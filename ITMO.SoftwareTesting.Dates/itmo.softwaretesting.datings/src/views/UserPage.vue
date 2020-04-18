<template>
	<b-container style="width: 500px">
		<b-input-group>
			<b-form-input
				v-model="password"
				placeholder="Пароль"
				type="password"
				trim
			></b-form-input>

			<b-input-group-append>
				<b-button variant="danger" v-on:click="deleteAccount()">
					Удалить аккаунт
				</b-button>
			</b-input-group-append>
		</b-input-group>
	</b-container>
</template>

<script>
	import {deleteAccount} from '../services/authentication.service';
	import {deleteToken} from '../tools/token';

	export default {
		name: 'UserPage',

		data() {
			return {
				password: '',
			};
		},

		methods: {
			async deleteAccount() {
				await deleteAccount(this.password);
				deleteToken();

				await this.$router.push({name: 'auth'});
			},
		},
	};
</script>

<style scoped>

</style>
