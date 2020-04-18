<template>
	<b-container class="px-4 mw-100 text-center">
		<b-container class="d-flex py-4" v-if="$router.currentRoute.name !== 'auth'">
			<b-nav tabs align="right" class="w-100 mx-3">
				<b-nav-item active>👪 Группы</b-nav-item>
				<b-nav-item>👀 Люди</b-nav-item>
				<b-nav-item>🎫 События</b-nav-item>
				<b-nav-item>🏫 Места</b-nav-item>
			</b-nav>

			<b-button-group class="ml-auto">
				<b-button variant="outline-secondary" class="px-4" to="/user">
					{{ nickname }}
				</b-button>

				<b-button type="sm" variant="outline-danger" v-on:click="deleteToken">
					Выйти
				</b-button>
			</b-button-group>
		</b-container>

		<router-view/>
	</b-container>
</template>

<script>
	import AuthPage from './views/AuthPage';
	import {deleteToken, nickname} from './tools/token';

	export default {
		name: 'app',
		components: {AuthPage},

		data: () => ({
			nickname: '',
		}),

		updated() {
			this.nickname = nickname();
		},

		mounted() {
			this.nickname = nickname();
		},

		methods: {
			async deleteToken() {
				deleteToken();
				await this.$router.push('/auth');
			},
		},
	};
</script>

