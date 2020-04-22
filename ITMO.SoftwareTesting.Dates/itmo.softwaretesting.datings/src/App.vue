<template>
	<b-container class="px-4 text-center min-vh-100">
		<b-container class="d-flex justify-content-end py-4" v-if="$router.currentRoute.name !== 'auth'">
			<b-nav tabs align="right" class="w-100 mx-3">
				<b-nav-item
					v-for="link in nav"
					:key="link.link"
					:active="$router.currentRoute.fullPath.includes(link.link)"
					:to="link.link"
				>
					{{ link.title }}
				</b-nav-item>
			</b-nav>

			<b-button-group>
				<b-button :variant="($router.currentRoute.fullPath.includes('/user') ? '' : 'outline-') + 'secondary'" to="/user" class="py-2 px-4" id="user-page-button">
					{{ nickname }}
				</b-button>

				<b-button variant="outline-danger" v-on:click="deleteToken" id="logout-button">
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
				{link: '/dates', title: '👀 Встречи'},
				{link: '/groups', title: '👪 Группы'},
				{link: '/events', title: '🎫 События'},
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

