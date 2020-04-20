<template>
	<b-container class="px-4 mw-100 text-center">
		<b-container class="d-flex py-4" v-if="$router.currentRoute.name !== 'auth'">
			<b-nav tabs align="right" class="w-100 mx-3">
				<b-nav-item
					v-for="link in nav"
					:key="link.link"
					:active="$router.currentRoute.fullPath === link.link"
					:to="link.link"
				>
					{{ link.title }}
				</b-nav-item>
			</b-nav>

			<b-button-group class="ml-auto">
				<b-button variant="outline-secondary" to="/user" class="py-2 px-4">
					{{ nickname }}
				</b-button>

				<b-button variant="outline-danger" v-on:click="deleteToken">
					Выйти
				</b-button>
			</b-button-group>
		</b-container>

		<router-view/>
	</b-container>
</template>

<script>
	import {deleteToken, nickname} from './tools/token';

	export default {
		name: 'app',

		data: () => ({
			nickname: '',
			nav: [
				{link: '/groups', title: '👪 Группы'},
				// {link: '/people', title: '👀 Люди'},
				{link: '/events', title: '🎫 События'},
				{link: '/places', title: '🏫 Места'},
			],
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

