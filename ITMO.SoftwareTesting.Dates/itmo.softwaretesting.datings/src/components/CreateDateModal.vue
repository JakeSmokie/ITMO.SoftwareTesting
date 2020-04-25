<template>
	<b-modal id="create-date-modal" hide-footer v-on:show="selectedGroup = null">
		<template v-slot:modal-title>
			Создать встречу
		</template>

		<h6>Выберите группу</h6>

		<b-list-group>
			<b-list-group-item
				action
				v-for="group in availableGroups" :key="group.id"
				:active="selectedGroup === group.id"
				@click="selectedGroup = group.id"
				class="modal-group"
			>
				{{ group.name }}
			</b-list-group-item>
		</b-list-group>

		<div class="d-flex justify-content-end">
			<b-button-group>
				<b-button variant="outline-danger" class="mt-3" @click="$bvModal.hide('create-date-modal')">
					Отменить
				</b-button>
				<b-button
					variant="outline-success" class="mt-3" :disabled="!selectedGroup"
					@click="createDate([selectedGroup, selectedEvent]), $bvModal.hide('create-date-modal')"
					id="modal-create-date-button"
				>
					Создать
				</b-button>
			</b-button-group>
		</div>
	</b-modal>
</template>
<script>
	import {mapActions, mapGetters, mapState} from 'vuex';

	export default {
		name: 'create-date-modal',
		data: () => ({
			selectedGroup: null,
		}),

		computed: {
			...mapGetters('groups', ['myGroups']),
			...mapGetters(['availableGroups']),
			...mapState('events', ['selectedEvent']),
			...mapState('dates', ['dates']),
		},

		methods: {
			...mapActions('events', ['loadFavoriteEvents']),
			...mapActions('groups', ['listGroups']),
			...mapActions('kudago', ['loadEventCategories', 'loadLocations']),
			...mapActions('events', ['loadEvents', 'selectEvent']),
			...mapActions('dates', ['createDate', 'loadDates']),
		},
	};
</script>
