<template>
	<div class="mt-4 text-left" v-if="invitations.length > 0">
		<h4 class="px-4">Приглашения в группы:</h4>

		<b-list-group class="pt-2">
			<b-list-group-item v-for="group in invitations" :key="group.id" class="group-foreign-invitation">
				<b-row class="text-left align-items-center">
					<b-col cols="3">
						📖 Название: "<span class="group-name">{{ group.name }}</span>"
					</b-col>
					<b-col>
						🧭 Назначение: "<span class="group-purpose">{{ group.purpose }}</span>"
					</b-col>
					<b-col cols="auto">
						<b-button
							size="sm" variant="outline-primary" v-on:click="acceptInvitation(group)"
							class="group-invitation-acceptance-button"
						>
							Принять приглашение
						</b-button>
					</b-col>
				</b-row>
			</b-list-group-item>
		</b-list-group>
	</div>
</template>

<script>
	import {mapActions, mapState} from 'vuex';

	export default {
		name: 'GroupInvitations',

		data: () => ({}),

		async created() {
			await this.loadGroupInvitations();
		},

		computed: {
			...mapState('groups', ['invitations']),
		},

		methods: {
			...mapActions('groups', ['loadGroupInvitations', 'acceptInvitation']),
		},
	};
</script>

<style scoped>

</style>
